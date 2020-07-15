import actionTypes from './actionTypes';
import { storeUserInfo, removeUserInfo } from './../../shared/utils';
import SignUp from './../../routes/signUp/index';

export function LogInAction(token, userId, username) {
    console.log(token);
    let user = {
        token: token,
        userId: userId,
        username: username
    };
    storeUserInfo(user);
    return {
        type: actionTypes.LOGIN,
        token: token,
        userId: userId,
        username: username
    };
};

export function LogOutAction() {
    removeUserInfo();
    return {
        type: actionTypes.LOGOUT,
        token: null
    };
};

export function SignUpAction() {
    removeUserInfo();
    return {
        type: actionTypes.SignUp,
        token: null
    };
};