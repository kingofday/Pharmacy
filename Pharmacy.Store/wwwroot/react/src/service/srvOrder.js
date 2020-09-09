//import strings from './../shared/constant';
import apiOrder from './../api/apiOrder';
import srvUser from './srvUser';

export default class srvOrder {
    static async add(order) {
        let user = srvUser.getUserInfo();
        if (!user.success) return user;
        return srvUser.checkResponse(await apiOrder.submit(user.result.token, order));
    }

    static async getHistory(pagenumber) {
        let user = srvUser.getUserInfo();
        if (!user.success) return user;
        return srvUser.checkResponse(await apiOrder.getHistory(user.result.token,pagenumber));
    }
}