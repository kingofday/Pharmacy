import CryptoJS from 'crypto-js';
import strings from '../shared/constant';
import userApi from '../api/apiUser';

export default class srvAuth {
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
    static async signIn(model) {
        let rep = await userApi.signIn(model);
        if (rep.success && rep.result.isConfirmed)
            this.storeUserInfo(rep.result);
        return rep
    }
    static async signUp(model) {
        return await userApi.signUp(model);
    }
    static async confirm(mobileNumber, code) {
        let conf = await userApi.confirm(mobileNumber, code);
        if (conf.success)
            this.storeUserInfo(conf.result);
        return conf;
    }
    static async resendSMS(mobileNumber) {
        return await userApi.resendSMS(mobileNumber);
    }
}