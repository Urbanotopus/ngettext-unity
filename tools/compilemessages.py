#!/usr/bin/env python
# based on django's compilemessages command, available here:
#   https://github.com/django/django/blob/eac9ab7ebb1c/django/core/management/commands/compilemessages.py

import codecs
import locale
from subprocess import Popen, PIPE
from typing import Tuple, Iterable, List

import concurrent.futures
import glob
import os
import sys


MSGFMT = 'msgfmt'
MSGFMT_OPTIONS = ['--check-format']

COMPILED_DOMAIN_SUFFIX = '_mo.bytes'
POT_FILE_SUFFIX = '.po'


def get_system_encoding():
    """The encoding of the default system locale. Fallback to 'ascii' if the
    #encoding is unsupported by Python or could not be determined. See tickets
    #10335 and #5846."""
    try:
        encoding = locale.getdefaultlocale()[1] or 'ascii'
        codecs.lookup(encoding)
    except Exception:
        encoding = 'ascii'
    return encoding


DEFAULT_LOCALE_ENCODING = get_system_encoding()


def popen_wrapper(args: List, stdout_encoding: str='utf-8'):
    """Friendly wrapper around Popen.
    Return stdout output, stderr output, and OS status code."""
    try:
        p = Popen(
            args,
            shell=False, stdout=PIPE, stderr=PIPE, close_fds=os.name != 'nt')
    except OSError as err:
        raise RuntimeError('Error executing %s (%s)' % (args[0], str(err)))
    output, errors = p.communicate()
    return (
        output.decode(stdout_encoding),
        errors.decode(DEFAULT_LOCALE_ENCODING, errors='replace'),
        p.returncode)


def generate_msgfmt_call(base_path):
    input_path = base_path + POT_FILE_SUFFIX
    output_path = base_path + COMPILED_DOMAIN_SUFFIX
    return [MSGFMT] + MSGFMT_OPTIONS + ['-o', output_path, input_path]


def compile_messages(locations: Iterable[Tuple[str, str]]):
    """Compile the message pot files to a `_mo.bytes` file."""
    with concurrent.futures.ThreadPoolExecutor() as executor:
        futures = []
        for i, (dirpath, f) in enumerate(locations):
            sys.stdout.write('processing file %s in %s\n' % (f, dirpath))
            po_path = os.path.join(dirpath, f)
            base_path = os.path.splitext(po_path)[0]
            args = generate_msgfmt_call(base_path)
            futures.append(executor.submit(popen_wrapper, args))

        for future in concurrent.futures.as_completed(futures):
            output, errors, status = future.result()
            if status:
                if errors:
                    sys.stderr.write('Execution of %s failed: %s' % (MSGFMT, errors))
                else:
                    sys.stderr.write('Execution of %s failed' % MSGFMT)


def main(source_directory: str=None):
    if source_directory is None:
        source_directory = os.getcwd()

    found_dirs = []
    for dirpath, dirnames, filenames in os.walk(source_directory, topdown=True):
        for dirname in dirnames:
            if dirname.lower() == 'locale':
                found_dirs.append(os.path.join(dirpath, dirname))

    locales = []
    for found_dir in found_dirs:
        locale_dirs = filter(os.path.isdir, glob.glob('%s/*' % found_dir))
        locales.extend(map(os.path.basename, locale_dirs))

    for found_dir in found_dirs:
        dirs = [
            os.path.join(found_dir, locale_name, 'LC_MESSAGES')
            for locale_name in locales]
        locations = []
        for possible_locale_dir in dirs:
            for dirpath, dirnames, filenames in os.walk(possible_locale_dir):
                locations.extend(
                    (dirpath, f)
                    for f in filenames if f.endswith(POT_FILE_SUFFIX))
        compile_messages(locations)


if __name__ == '__main__':
    main(*sys.argv[1:])
