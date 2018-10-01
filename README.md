# NGettext for Unity

Implementation of the NGettext .NET implementation of the GNU/Gettext library for Unity.

## Installation
### Requirements
First of all, you need to install `NGettext` through `NuGet`. For example:
```
PM> Install-Package NGettext
```

### Project Files
Now, copy or clone the project to your Unity assets folder.


## Example Usage
### Assuming the following files
We assume you use the default domain `messages` 
and thus have a `messages.po` file that is your French pot file. As follows:
`YourUnityProject/Assets/Resources/Locale/fr-FR/LC_MESSAGES/messages.po`

With this content for example:

```po
msgid ""
msgstr ""
"Project-Id-Version: master\n"
"POT-Creation-Date: 2018-08-06 07:52-0500\n"
"PO-Revision-Date: 2018-10-01 21:49+0200\n"
"MIME-Version: 1.0\n"
"Content-Type: text/plain; charset=UTF-8\n"
"Content-Transfer-Encoding: 8bit\n"
"Plural-Forms: nplurals=2; plural=(n > 1);\n"
"Last-Translator: \n"
"Language: fr_FR\n"

msgid "Hello, {0}!"
msgstr "Heyo, {0}!"
```

Now, you need to compile the pot file to a `mo` file as `domain_mo.bytes`, as follows:
`YourUnityProject/Assets/Resources/Locale/fr-FR/LC_MESSAGES/messages_mo.bytes`.


### Unity Controller Side
Now in Unity, you could do this:

```cs
NGettextUnity.Instance.LoadLocale("fr-FR");
Debug.Log(NGettextUnity.Catalog.GetString("Hello, {0}!", "World"));
```

If done successfully, Unity should print: `Heyo, World!`.
