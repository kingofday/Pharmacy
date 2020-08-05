import actionTypes from './actionTypes';

export function LogInAction(user) {
    return {
        type: actionTypes.LOGIN,
        payload: { ...user }
    };
};

export function LogOutAction() {
    return {
        type: actionTypes.LOGOUT
    };
};

export function SetNexPage(nextPage) {
    return {
        type: actionTypes.SignUp,
        payload: { nextPage: nextPage }
    };
};