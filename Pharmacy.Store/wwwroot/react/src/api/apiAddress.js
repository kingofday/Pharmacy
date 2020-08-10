import addr from './addresses';
import strings from './../shared/constant';

export default class apiAddress {
    static async get(token) {
        let url = addr.getAddresses;
        var handleResponse = async (response) => {
            const rep = await response.json();
            if (!rep.IsSuccessful)
                return { success: false, message: rep.Message };
            else return {
                success: true,
                result: rep.Result.Items.map((a) => ({
                    id: a.Id,
                    fullname:a.fullname,
                    mobileNumber:a.MobileNumber,
                    details: a.Details,
                    lat: a.Lat,
                    lng: a.Lng
                }))
            }
        }
        try {
            const response = await fetch(url, {
                method: 'GET',
                mode: 'cors',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8;',
                    'Authorize':`bearer ${token}`
                }
            });
            return await handleResponse(response);
        } catch (error) {
            console.log(error);
            return ({ success: false, message: strings.connectionFailed });
        }
    }

}