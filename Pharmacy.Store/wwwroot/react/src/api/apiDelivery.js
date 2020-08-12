import addr from './addresses';
import strings from './../shared/constant';

export default class apiDelivery {
    static async get(token) {
        let url = addr.getDeliveries;
        var handleResponse = async (response) => {
            const rep = await response.json();
            if (!rep.IsSuccessful)
                return { success: false, message: rep.Message };
            else return {
                success: true,
                result: rep.Result.map((d) => ({
                    id: d.Id,
                    name: d.Name
                }))
            }
        }
        try {
            const response = await fetch(url, {
                'method': 'GET',
                'mode': 'cors',
                'headers': {
                    'Content-Type': 'application/json; charset=utf-8;'
                }
            });
            return await handleResponse(response);
        } catch (error) {
            console.log(error);
            return ({ success: false, message: strings.connectionFailed });
        }
    }


}