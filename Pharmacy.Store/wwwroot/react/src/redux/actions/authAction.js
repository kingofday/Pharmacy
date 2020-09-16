import actionTypes from './actionTypes';

export function LogInAction(user) {
    return {
        type: actionTypes.LOG_IN,
        payload: { ...user }
    };
};

export function UpdateProfileAction(user) {
    return {
        type: actionTypes.UPDATE_PROFILE,
        payload: { ...user }
    };
};

export function LogOutAction() {
    return {
        type: actionTypes.LOG_OUT
    };
};

export function SetNexPage(nextPage) {
    return {
        type: actionTypes.SET_AUTH_NEXT_PAGE,
        payload: { nextPage: nextPage }
    };
};
export function GoToNextPage() {
    return {
        type: actionTypes.AUTH_GOTO_NEXT_PAGE,
        payload: { goToNextPage: true }
    };
};