import CryptoJS from 'crypto-js';

export default class userSrv{
    static getUserInfo() {
        let ciphertext = localStorage.getItem('user');
        if (ciphertext == null)
            return { success: false, message: strings.notAutheticated, status: 401 };
        var bytes = CryptoJS.AES.decrypt(ciphertext, 'kingofday.ir');
        var user = JSON.parse(bytes.toString(CryptoJS.enc.Utf8));
        return {
            success: true,
            result: user
        }
    }
    
    static storeUserInfo(user) {
        let userInfo = JSON.stringify(user);
        let encInfo = CryptoJS.AES.encrypt(userInfo, 'kingofday.ir').toString();
        localStorage.setItem('user', encInfo);
    }
    
    static removeUserInfo() {
        localStorage.removeItem('user');
    }
}