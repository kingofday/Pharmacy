//import strings from './../shared/constant';
import apiOrder from './../api/apiOrder';
import srvAuth from './srvAuth';

export default class srvOrder {
    static async add(order) {
        let user = srvAuth.getUserInfo();
        if (!user.success) return user;
        return srvAuth.checkResponse(await apiOrder.submit(user.result.token, order));
    }
}