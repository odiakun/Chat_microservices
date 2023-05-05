//connection to entrypoint
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr'

export default {
    createHub() {
        return new HubConnectionBuilder()
            .withUrl("http://entrance.hpds" + "/chat")
            .configureLogging(LogLevel.Information)
            .build();
    }
}