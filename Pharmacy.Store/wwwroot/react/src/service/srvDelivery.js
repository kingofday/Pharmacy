import apiDrug from './../api/apiDelivery';
import srvAuth from './srvAuth';
export default class srvDelivery {
    static async get() {
        return srvAuth.checkResponse(await apiDrug.get());
    }
}