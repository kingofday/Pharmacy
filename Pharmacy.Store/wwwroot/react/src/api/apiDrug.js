import addr from './addreses';
import strings from './../shared/constant';

export default class apiDrug {
    static async search(q) {
        var handleResponse = async (response) => {
            const rep = await response.json();
            if (!rep.IsSuccessful)
                return { success: false, message: rep.Message }
            else return {
                success: true,
                result: rep.Result.Items.map((c) => ({
                    id: c.CategoryId,
                    name: c.Name
                }))
            }
        }
        let url = `${addr.getCategories}?pageSize?pageNumber=${pageNumber}&pageSize=${pageSize}`;
        try {
            const response = await fetch(url, {
                method: 'GET',
                mode: 'cors',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8;'
                }
            });
            return await handleResponse(response);
        } catch (error) {
            if ('caches' in window) {
                let data = await caches.match(url);
                if (data)
                    return await handleResponse(data);
                else return ({ success: false, message: strings.connecttionFailed });
            }
            else return ({ success: false, message: strings.connecttionFailed });
        }
    }
}