import apiPrescription from './../api/apiPrescription';
import srvAuth from './srvAuth';

export default class srvAddress {
    static async add(mobileNumber, files) {
        let user = srvAuth.getUserInfo();
        return srvAuth.checkResponse(await apiPrescription.submit(user.success ? user.result.token : null, mobileNumber, files));
    }
}