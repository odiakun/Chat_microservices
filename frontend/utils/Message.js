//klasa wiadomosci i jej atrybuty
class Message {
    constructor(messid, mid, user, text) {
        this.messid = messid;
        this.mid = mid;
        this.user = user;
        this.text = text;
        this.timestamp = Math.floor(new Date().getTime()/1000.0)
    }
}
export default Message;