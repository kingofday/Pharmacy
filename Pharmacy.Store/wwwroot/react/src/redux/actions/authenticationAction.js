import actionTypes from './actionTypes';

export function LogInAction(token, userId, username) {
    let user = {
        token: token,
        userId: userId,
        username: username
    };
    return {
        type: actionTypes.LOGIN,
        token: token,
        userId: userId,
        username: username
    };
};

export function LogOutAction() {
    return {
        type: actionTypes.LOGOUT,
        token: null
    };
};

export function SignUpAction() {
    return {
        type: actionTypes.SignUp,
        token: null
    };
};