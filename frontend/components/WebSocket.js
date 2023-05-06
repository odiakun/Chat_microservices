//connection to entrypoint
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr'

export default {
    createHub(url) {
        return new HubConnectionBuilder()
            .withUrl(url)
            .configureLogging(LogLevel.Information)
            .build();
    }
}