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
                        url: rep.Result.Url,
                        drugs: rep.Result.Drugs ? rep.Result.Drugs.map((p) => ({
                            id: p.Id,
                            price: p.Price,
                            discount: p.Discount,
                            count: p.Count
                        })) : []
                    }
                };
            else return { success: false, message: rep.Message, status: rep.Status };
        }
        catch (error) {
            console.log(error);
            return { success: false, message: strings.connecttionFailed };
        }
    }

}