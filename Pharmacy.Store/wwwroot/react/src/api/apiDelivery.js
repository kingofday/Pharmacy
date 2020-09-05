import addr from './addresses';
import strings from './../shared/constant';

export default class apiDelivery {
    static async get(token) {
        let url = addr.getDeliveries;
        var handleResponse = async (response) => {
            const rep = await response.json();
            if (!rep.IsSuccessful)
                return { success: false, message: rep.Message, status: rep.Status };
            else return {
                status: 200,
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
                    'Content-Type': 'application/json; charset=utf-8;',
                    'Authorization': `Bearer ${token}`
                }
            });
            return await handleResponse(response);
        } catch (error) {
            console.log(error);
            return ({ success: false, message: strings.connectionFailed });
        }
    }

    static async getPrice(id) {
        let url = addr.getDeliveryPrice(id);
        var handleResponse = async (response) => {
            const rep = await response.json();
            if (!rep.IsSuccessful)
                return { success: false, message: rep.Message, status: rep.Status };
            else return {
                status: 200,
                success: true,
                result: {
                    uniqueId: rep.Result.UniqueId,
                    price: rep.Result.Price
                }
            }
        }
        try {
            const response = await fetch(url, {
                'method': 'GET',
                'mode': 'cors',
                'headers': {
                    'Content-Type': 'application/json; charset=utf-8;',
                }
            });
            return await handleResponse(response);
        } catch (error) {
            console.log(error);
            return ({ success: false, message: strings.connectionFailed });
        }
    }
}