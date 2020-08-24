import addr from './addresses';
import strings from './../shared/constant';

export default class apiAddress {
    static async get(token) {
        let url = addr.getAddresses;
        var handleResponse = async (response) => {
            const rep = await response.json();
            if (!rep.IsSuccessful)
                return { success: false, message: rep.Message, status: rep.Status };
            else return {
                status: 200,
                success: true,
                result: rep.Result.map((a) => ({
                    id: a.Id,
                    fullname: a.Fullname,
                    mobileNumber: a.MobileNumber,
                    details: a.Details,
                    lat: a.Lat,
                    lng: a.Lng
                }))
            }
        }
        try {
            const response = await fetch(url, {
                'method': 'GET',
                'mode': 'cors',
                //'credentials': 'include',
                'headers': {
                    'Content-Type': 'application/json; charset=utf-8;',
                    //'Content-Type':'application/x-www-form-urlencoded',
                    'Authorization': `Bearer ${token}`
                }
            });
            return await handleResponse(response);
        } catch (error) {
            console.log(error);
            return ({ success: false, message: strings.connectionFailed });
        }
    }

    static async add(token, address) {
        let url = addr.addAddress;
        var handleResponse = async (response) => {
            const rep = await response.json();
            if (!rep.IsSuccessful)
                return { success: false, message: rep.Message, status: rep.Status };
            else return {
                status: 200,
                success: true,
                result: rep.Result
            }
        }
        try {
            const response = await fetch(url, {
                'method': 'POST',
                'mode': 'cors',
                'headers': {
                    'Content-Type': 'application/json; charset=utf-8;',
                    'Authorization': `Bearer ${token}`
                },
                'body': JSON.stringify(address)
            });
            return await handleResponse(response);
        } catch (error) {
            console.log(error);
            return ({ success: false, message: strings.connectionFailed });
        }
    }

    static async update(token, address) {
        let url = addr.addAddress;
        var handleResponse = async (response) => {
            const rep = await response.json();
            if (!rep.IsSuccessful)
                return { success: false, message: rep.Message, status: rep.Status };
            else return {
                status: 200,
                success: true,
                result: rep.Result
            }
        }
        try {
            const response = await fetch(url, {
                'method': 'PUT',
                'mode': 'cors',
                'headers': {
                    'Content-Type': 'application/json; charset=utf-8;',
                    'Authorization': `Bearer ${token}`
                },
                'body': JSON.stringify(address)
            });
            return await handleResponse(response);
        } catch (error) {
            console.log(error);
            return ({ success: false, message: strings.connectionFailed });
        }
    }
}