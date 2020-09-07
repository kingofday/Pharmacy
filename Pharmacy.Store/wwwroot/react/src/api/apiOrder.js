import addr from './addresses';
import strings from './../shared/constant';

export default class apiOrder {

    static async submit(token, order) {
        try {
            const response = await fetch(addr.addOrder, {
                method: 'POST',
                mode: 'cors',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8;',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(order) // body data type must match "Content-Type" header
            });
            const rep = await response.json();
            if (rep.IsSuccessful)
                return {
                    status: 200,
                    success: true,
                    result: {
                        id: rep.Result.OrderId,
                        basketChanged: rep.Result.BasketChanged,
                        url: rep.Result.Url
                    }
                };
            else return {
                status: rep.Status,
                success: false,
                message: rep.Message || strings.connectionFailed,
                result: rep.Result ? {
                    basketChanged: rep.Result.BasketChanged,
                    drugs: rep.Result.Drugs ? rep.Result.Drugs.map((p) => ({
                        drugId: p.DrugId,
                        price: p.Price,
                        discount: p.DiscountPrice,
                        count: p.Count
                    })) : []
                } : null
            };
        }
        catch (error) {
            console.log(error);
            return { success: false, message: strings.connecttionFailed };
        }
    }

    static async getHistory(token) {
        let url = addr.getOrders;
        var handleResponse = async (response) => {
            const rep = await response.json();
            if (!rep.IsSuccessful)
                return { success: false, message: rep.Message, status: rep.Status };
            else return {
                status: 200,
                success: true,
                result: rep.Result.map((x) => ({
                    uniqueId: x.UniqueId,
                    status: x.Status,
                    totalPrice: x.TotalPrice,
                    insertDate: x.InsertDateSh,
                    items : x.map(oi => ({
                        nameFa: oi.Drug.NameFa,
                        discountPrice: oi.DiscountPrice,
                        price: oi.Price,
                        count: oi.Count,
                        totalPrice: oi.TotalPrice,
                        uniqueId: oi.Drug.UniqueId
                    }))
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
}