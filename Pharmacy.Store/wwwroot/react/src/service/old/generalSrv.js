import generalApi from './../api/generalApi';
import strings from './../shared/constant';

export default class generalSrv {
    static offsetInHours = 24;
    static contactUsKey = 'contactus';

    static async getContactUsInfo() {

        if (!localStorage) {

            return;
        }
        let callApi = async () => {
            let apiRep = await generalApi.getContactUsInfo();
            console.log(apiRep);
            if (apiRep.success) {
                let cdt = new Date();
                cdt.setHours(cdt.getHours() + this.offsetInHours);
                apiRep.result.expDateTime = cdt.getTime();
                localStorage.setItem(this.contactUsKey, JSON.stringify(apiRep.result));
            }
            return apiRep;
        };
        try {
            let jsonInfo = localStorage.getItem(this.contactUsKey);
            console.log(jsonInfo);
            if (!jsonInfo) return await callApi();

            let contactUs = JSON.parse(jsonInfo);
            if (contactUs.expDateTime < new Date().getTime()) return await callApi();
            return { success: true, result: contactUs };
        }
        catch{
            localStorage.removeItem(this.contactUsKey);
            return { success: false, message: strings.retryPlease };
        }
    }
}