//import strings from './../shared/constant';
import apiAddress from './../api/apiAddress';
import srvUser from './srvUser';

export default class srvAddress {
    static async get() {
        let user = srvUser.getUserInfo();
        if (!user.success) return user;
        return srvUser.checkResponse(await apiAddress.get(user.result.token));
    }
    static async add(addr) {
        let user = srvUser.getUserInfo();
        if (!user.success) return user;
        return srvUser.checkResponse(await apiAddress.add(user.result.token, addr));
    }
    static async update(addr) {
        let user = srvUser.getUserInfo();
        if (!user.success) return user;
        return srvUser.checkResponse(await apiAddress.update(user.result.token, addr));
    }
}