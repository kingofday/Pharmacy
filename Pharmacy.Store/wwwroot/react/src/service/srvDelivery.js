import apiDrug from './../api/apiDelivery';

export default class srvDelivery {
    static async get() {
        return await apiDrug.get();
    }
}