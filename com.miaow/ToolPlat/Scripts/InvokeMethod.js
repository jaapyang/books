function Invoke(methodName, handlerArgs) {
    var arg = {
        "MethodName": methodName,
        "ArgsJsonStr": handlerArgs
    };
    
    var argStr = JSON.stringify(arg);
    window.external.HandlerProcess(argStr);
}

function Start_New_Tag(toolName, methodName, handlerArgs) {
    var arg = {
        "MethodName": methodName,
        "ArgsJsonStr": handlerArgs
    };

    var argStr = JSON.stringify(arg);
    window.external.StartNewHandler(toolName,argStr);
}