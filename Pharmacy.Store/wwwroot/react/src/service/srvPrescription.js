import apiPrescription from './../api/apiPrescription';

export default class srvAddress {
    static async add(mobileNumber, files) {
        let user = srvAuth.getUserInfo();
        if (!user.success) return user;
        return await apiPrescription.submit(user.success ? user.result.token : null, mobileNumber, files);
    }
}