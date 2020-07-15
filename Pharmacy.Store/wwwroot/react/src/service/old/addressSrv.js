
export default class addressSrv {
    static addressKey = 'address_info';
    
    static saveInfo(reciever, recieverMobileNumber) {
        localStorage.setItem(this.addressKey, JSON.stringify({ reciever, recieverMobileNumber }));
    }

    static getInfo() {
        let info = localStorage.getItem(this.addressKey);
        if (info) return JSON.parse(info);
        else return null;
    }
}