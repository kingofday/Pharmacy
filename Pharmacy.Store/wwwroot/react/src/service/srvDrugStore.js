//import strings from './../shared/constant';
import apiDrugStore from './../api/apiDrugStore';

export default class drugStoreSrv {
    static async get() {
        return await apiDrugStore.get();
    }
}