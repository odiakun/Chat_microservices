//klasa wiadomosci i jej atrybuty
class CombinedMessage {
    constructor(messid, mid, user, text, url) {
        this.messid = messid;
        this.mid = mid;
        this.user = user;
        this.text = text;
        this.url = url;
        this.timestamp = Math.floor(new Date().getTime()/1000.0)
    }
}
export default CombinedMessage;