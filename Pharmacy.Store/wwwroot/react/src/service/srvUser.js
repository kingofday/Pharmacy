import CryptoJS from 'crypto-js';
import strings from '../shared/constant';
import userApi from '../api/apiUser';

export default class srvUser {

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
        console.log('removed');
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

    static checkResponse(rep) {
        if (!rep.success && rep.status === 401) {
            window.location.href = '/auth';
            return { ...rep, message: strings.loginAgain }
        }
        else return rep;
    }

    static async updateProfile(model) {
        let user = this.getUserInfo();
        if (!user.success) return user;
        let rep = this.checkResponse(await userApi.updateProfile(user.result.token, model));
        let getUser = this.getUserInfo();
        if (rep.success && getUser.success)
            this.storeUserInfo({ ...getUser.result, fullname: model.fullname, email: model.email });
        return rep
    }
}