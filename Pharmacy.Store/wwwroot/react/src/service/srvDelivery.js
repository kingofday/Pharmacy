import apiDel from './../api/apiDelivery';
import srvUser from './srvUser';
export default class srvDelivery {
    static async get() {
        return srvUser.checkResponse(await apiDel.get());
    }

    static async getPrice(id) {
        return await apiDel.getPrice(id);
    }

    static async payPrice(id) {
        return await apiDel.payPrice(id);
    }
}