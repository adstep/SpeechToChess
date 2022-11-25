# Speech To Chess

Play chess in the browser using voice commands.

A speech-to-chess console application that allows the user to play on the popular chess website Lichess using their voice. Supports running on Windows Desktop Speech technology, locally, or Azure Cognitive Services, if configured for remote.

## Usage

SpeechToChess.exe --help
SpeechToChess 1.0.0
Copyright (C) 2022 SpeechToChess

  -e, --engine    (Default: Local) Set speech recognition engine. [Local, Cognitive]

  --help          Display this help screen.

  --version       Display version information.


## Configuration

All configuration is done via an ```appsettings.local.json``` that should be adjacent ot the exe.

### **Cognitive**

Configuration for Azure Cognitive Services. Only necessary if running using ```Cognitive``` Speech recognition engine. To setup the necessary resources, you can follow the run-script ```./scripts/setup.ps1```.

**Key**: The Speech resource key the endpoint is associated with.

**Region**: The Azure region the endpoint is associated with.

**EndpointId**: The identifier of the endpoint.

Example:

```json
{
    "Cognitive": {
        "Key": "",
        "Region": "",
        "EndpointId": ""
    }
}
```

### **Lichess**

Configuration for logging onto a specified Lichess account.

**UserName**: Lichess userName.

**Passowrd**: Lichess password.

Example:
```json
{
    "Lichess": {
        "UserName": "",
        "Password":  ""
    }
}
```

## Supported voice commands

Valid chess moves in ```Algebraic Notation```:
* move {file} {rank} to {file} {rank}
* {file} {rank} [moves] to {file} {rank}
* move {piece} to {file} {rank}
* {piece} [moves] to {file} {rank}
* {piece} {file} {rank}
* king-side [castle]
* queen-side [castle]
* [promote] {file} eight [to] {piece}

A number of other commands are supported:
* ```clear```/```undo```: Clears lichess text input
* ```resign```: Inputs lichess resign command
* ```draw```: Inputs lichess draw command
* ```clock```/```time```: Inputs lichess clock command
* ```who```: Inputs lichess who command
* ```next```: Inputs lichess next puzzle command
* ```puzzle```: Navigates lichess to puzzle page
* ```home```: Navigates lichess to home page

See [algebraic-notation.md](./config/algebraic-notation.md) for complete description of supported commands. File is written in structured text data supported by Azure Cognitive Services. Read [structured-text-data-for-training](https://learn.microsoft.com/en-us/azure/cognitive-services/speech-service/how-to-custom-speech-test-and-train#structured-text-data-for-training) for in-depth description of format.
