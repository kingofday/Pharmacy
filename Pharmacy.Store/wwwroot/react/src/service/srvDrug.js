//import strings from './../shared/constant';
import apiDrug from './../api/apiDrug';

export default class drugSrv {
    static async get(filter) {
        return await apiDrug.get(filter);
    }

    static async getSingle(id) {
        return await apiDrug.getSingle(id);
    }
}