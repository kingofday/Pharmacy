import apiPrescription from './../api/apiPrescription';
import srvUser from './srvUser';

export default class srvAddress {
    static async add(mobileNumber, files) {
        let user = srvUser.getUserInfo();
        return srvUser.checkResponse(await apiPrescription.submit(user.success ? user.result.token : null, mobileNumber, files));
    }

    static async getItems(id) {
        return await apiPrescription.getItems(id);
    }
}