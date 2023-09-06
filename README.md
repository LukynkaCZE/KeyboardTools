# Keyboard Tools

This is collection of keyboard utilities I wrote for my own use!
It can replace keys with custom strings and run commands!

## How to Install

Download the [latest release](https://github.com/LukynkaCZE/KeyboardTools/releases/latest) and run it! It will automatically move the executable to `User/Documents/KeyboardTools` along with adding the executable to PATH so you can run it from anywhere!

## How to run/stop

- To start the program, run `KeyboardTools start` in your terminal (Starting the program will create new process so you can close the current terminal window. If you want to run it in the current terminal window, run `KeyboardTools start -bg`)
- To stop it, run `KeyboardTools stop`.  

## Config

Config is located in `User/Documents/KeyboardTools/config.json`. When starting for the first time it will generate template config showing some basic functionallity. You can modify it however you want.

For each key, you need following json object:
```json
    {
      "key": "<key>",
      "replacement": "<replacement>",
      "type": "<type>",
      "modKey": "<modifier key>"
    }
```
- `key` is the key that will be replaced/listened to. [Here](https://github.com/MediatedCommunications/WindowsInput/blob/859f8d0b061582a986d499e25ebd5e1e08f29082/WindowsInput/Events/Keyboard/KeyCode.cs) is list of all supported keys and their format
- `replacement` is the string the key will be replaced with. If `type` is `COMMAND`, this is what will get run as command
- `type` is the type. There are currently only two types: `REPLACE` and `COMMAND`
  - `REPLACE` will act as string replacement for key
  - `COMMAND` will run whatever is in `replacement` field as command in new cmd process
- `modKey` is the modifier key; It uses the same list of keys as above. Do keep in mind that this is **not** limited to usual modifier keys such as `LShift`, `RCtrl` etc. so you can use any key you want as modifier! **Keep the field empty if you don't want to use mod key**

---

### Here are some config examples:

This replaces `LShift + Q` with `;` because I am czech and there is no `;` on default czech keyboard
```json
    {
      "key": "Q",
      "replacement": ";",
      "type": "REPLACE",
      "modKey": "LShift"
    }
```

This would run `shutdown /h` (System hybernate) when `LCtrl + H` is pressed
```json
    {
      "key": "H",
      "replacement": "shudown /h",
      "type": "COMMAND",
      "modKey": "LCtrl"
    }
```

This would type `Trans Rights! 🏳️‍⚧️` when `T + R` are pressed
```json
    {
      "key": "T",
      "replacement": "Trans Rights! 🏳️‍⚧️",
      "type": "REPLACE",
      "modKey": "R"
    }
```
