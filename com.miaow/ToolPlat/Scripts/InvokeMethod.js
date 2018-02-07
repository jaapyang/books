function Invoke(methodName, handlerArgs) {
    
    var arg = {
        "MethodName": methodName,
        "ArgsJsonStr": handlerArgs
    };
    
    var argStr = JSON.stringify(arg);
    window.external.HandlerProcess(argStr);
}