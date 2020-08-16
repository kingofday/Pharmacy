//import strings from './../shared/constant';
import apiAddress from './../api/apiAddress';
import srvAuth from './srvAuth';

export default class srvAddress {
    static async get() {
        let user = srvAuth.getUserInfo();
        if (!user.success) return user;
        return srvAuth.checkResponse(await apiAddress.get(user.result.token));
    }
    static async add(addr) {
        let user = srvAuth.getUserInfo();
        if (!user.success) return user;
        return srvAuth.checkResponse(await apiAddress.add(user.result.token, addr));
    }
    static async update(addr) {
        let user = srvAuth.getUserInfo();
        if (!user.success) return user;
        return srvAuth.checkResponse(await apiAddress.update(user.result.token, addr));
    }
}