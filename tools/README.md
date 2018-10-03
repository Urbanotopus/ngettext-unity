# NGettext for Unity: Tools


### Compiling GNU pot file messages
`compilemessages.py` looks for `Locale` folders (insensitive) 
in a given path or if not given, in the current working directory.

If some pot files (`.po`) were found, it compiles them using `msgfmt` 
to a Unity readable bytes file which thus ends by `_mo.bytes`.


## **(Windows Only)** Requirements to run `compilemessages`
Please, if you are a Windows user, make sure to install 
[gettext-iconv-windows](https://github.com/mlocati/gettext-iconv-windows/releases/download/v0.19.8.1-v1.15/gettext0.19.8.1-iconv1.15-static-64.exe)
so the tool can use GNU's gettext tools.


## Usage
```
$ python3 compilemessages.py [SOURCE_DIRECTORY]
```


## Example usage with an Unity project
```bash
$ python3 tools/compilemessages.py 'C:\Users\Me\Documents\MyUnityProject\Assets'
processing file messages.po in C:\Users\Me\Documents\MyUnityProject\Asset\Resources\Locale\fr-FR\LC_MESSAGES
```
