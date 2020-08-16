import addr from './addresses';
import strings from './../shared/constant';

export default class apiPrescription {

    static async submit(token, mobileNumber, files) {
        try {
            let data = new FormData();
            let headers = {};
            if (token) headers['Authorization'] = `Bearer ${token}`;
            else data.append('mobileNumber', mobileNumber);
            files.forEach((file) => data.append('files', file, file.name));
            const response = await fetch(addr.addOrder, {
                method: 'POST',
                mode: 'cors',
                headers: headers,
                body: data
            });
            const rep = await response.json();
            if (rep.IsSuccessful)
                return {
                    status: 200,
                    success: true,
                    result: rep.Result
                };
            else return { success: false, message: rep.Message, status: rep.Status };
        }
        catch (error) {
            console.log(error);
            return { success: false, message: strings.connecttionFailed };
        }
    }

}