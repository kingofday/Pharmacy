import apiDel from './../api/apiDelivery';
import srvAuth from './srvAuth';
export default class srvDelivery {
    static async get() {
        return srvAuth.checkResponse(await apiDel.get());
    }

    static async getPrice(id) {
        return await apiDel.getPrice(id);
    }
}