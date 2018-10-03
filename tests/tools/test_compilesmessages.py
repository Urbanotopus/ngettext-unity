from types import ModuleType
from unittest import TestCase
from os.path import join, isdir, isfile

import mock

from tests.tools.constants import HERE_PATH, TOOLS_PATH


TEST_PROJECT_PATH = join(HERE_PATH, 'test_project')
COMPILE_MESSAGES_PATH = join(TOOLS_PATH, 'compilemessages.py')


assert isdir(TEST_PROJECT_PATH), 'Test suite is missing the test project'
assert isfile(COMPILE_MESSAGES_PATH), 'Test suite is missing `compile messages`'


def _load_pyfile(path, name):
    with open(path, mode='rb') as fp:
        loaded_pyfile = ModuleType(name)
        exec(compile(fp.read(), path, 'exec'), loaded_pyfile.__dict__)
    return loaded_pyfile


class TestCompileMessages(TestCase):
    compile_messages_module = _load_pyfile(
        COMPILE_MESSAGES_PATH, 'compilemessages')

    def _generate_msgfmt(self, source_path):
        full_path = join(TEST_PROJECT_PATH, source_path)
        args = self.compile_messages_module.generate_msgfmt_call(full_path)

        self.assertGreater(len(args), 0)
        self.assertTrue(args[-1].endswith('.po'))
        self.assertTrue(args[-2].endswith(
            self.compile_messages_module.COMPILED_DOMAIN_SUFFIX))

        return mock.call(args)

    @mock.patch.object(compile_messages_module, 'compile_messages')
    def test_compile_messages_invalid_path(self, mocked_compile_messages):
        """Ensure the project doesn't attempt to compile
        non-existing locales or files."""
        self.compile_messages_module.main(TEST_PROJECT_PATH + '__invalid')
        assert mocked_compile_messages.call_count == 0

    @mock.patch.object(compile_messages_module, 'popen_wrapper')
    def test_compile_messages_valid_path(self, mocked_popen_wrapper: mock.Mock):
        """Ensure locale files are found and msgfmt is correctly called."""
        expected_calls = [
            self._generate_msgfmt(
                join('assets', 'Locale', 'en-US', 'LC_MESSAGES', 'messages')),
            self._generate_msgfmt(
                join('assets', 'Locale', 'fr-FR', 'LC_MESSAGES', 'messages')),
            self._generate_msgfmt(
                join('assets', 'Locale', 'fr-FR', 'LC_MESSAGES', 'specials'))]
        mocked_popen_wrapper.return_value = (None, None, None)

        self.compile_messages_module.main(TEST_PROJECT_PATH)
        mocked_popen_wrapper.assert_has_calls(expected_calls, any_order=True)
