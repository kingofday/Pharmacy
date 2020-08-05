import addr from './addresses';
import strings from './../shared/constant';

export default class apiDrug {

    static async signUp(model) {
        try {
            const response = await fetch(addr.signUp, {
                method: 'POST',
                mode: 'cors',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8;'
                },
                body: JSON.stringify(model)
            });
            const rep = await response.json();
            if (!rep.IsSuccessful)
                return { success: false, message: rep.Message };
            console.log(rep.Result);
            let user = rep.Result;
            return {
                success: true,
                result: rep.Result.MobileNumber
            }
        } catch (error) {
            return ({ success: false, message: strings.connectionFailed });
        }
    }

    static async confirm(model) {
        try {
            const response = await fetch(addr.confirm(model.mobileNumber), {
                method: 'POST',
                mode: 'cors',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8;'
                },
                body: JSON.stringify(model)
            });
            const rep = await response.json();
            if (!rep.IsSuccessful)
                return { success: false, message: rep.Message };
            console.log(rep.Result);
            let user = rep.Result;
            return {
                success: true,
                result: {
                    email: user.Email,
                    mobileNumber: user.MobileNumber,
                    fullname: user.Fullname,
                    token: user.Token,
                    isConfirmed: user.IsConfirmed
                }
            }
        } catch (error) {
            return ({ success: false, message: strings.connectionFailed });
        }
    }
}