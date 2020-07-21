//import strings from './../shared/constant';
import apiDrug from './../api/apiDrug';

export default class drugSrv {
    static async search(q){
        return await apiDrug.search(q);
    }
}