from os.path import abspath, dirname, pardir, join


HERE_PATH = abspath(join(dirname(__file__)))
TOOLS_PATH = abspath(join(HERE_PATH, pardir, pardir, 'tools'))
