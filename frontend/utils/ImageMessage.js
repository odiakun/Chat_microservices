class ImageMessage {
    constructor(messid, mid, user, base64) {
        this.messid = messid;
        this.mid = mid;
        this.user = user;
        this.base64 = base64;
        this.timestamp = Math.floor(new Date().getTime()/1000.0)
    }
}
export default ImageMessage;