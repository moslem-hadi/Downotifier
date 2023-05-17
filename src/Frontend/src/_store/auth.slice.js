import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';

import { history, fetchWrapper } from '../_helpers';

// create slice

const name = 'auth';
const initialState = createInitialState();
const reducers = createReducers();
const extraActions = createExtraActions();
const extraReducers = createExtraReducers();
const slice = createSlice({ name, initialState, reducers, extraReducers });

// exports

export const authActions = { ...slice.actions, ...extraActions };
export const authReducer = slice.reducer;

// implementation

function createInitialState() {
  return {
    // initialize state from local storage to enable user to stay logged in
    user: JSON.parse(localStorage.getItem('user')),
    error: null,
  };
}

function createReducers() {
  return {
    logout,
  };

  function logout(state) {
    state.user = null;
    localStorage.removeItem('user');
    history.navigate('/login');
  }
}

function createExtraActions() {
  const baseUrl = `${process.env.REACT_APP_AUTH_URL}`;

  return {
    login: login(),
  };

  function login() {
    return createAsyncThunk(
      `${name}/login`,
      async ({ username, password }) => {
        var myHeaders = new Headers();
        myHeaders.append("Content-Type", "application/x-www-form-urlencoded");

        var urlencoded = new URLSearchParams();
        urlencoded.append("client_id", "JobsWebApi");
        urlencoded.append("grant_type", "password");
        urlencoded.append("username", username);
        urlencoded.append("password", password);
        urlencoded.append("client_secret", "49C1A7E1-0C79-4A89-A3D6-A37998FB86B0");

        var requestOptions = {
          method: 'POST',
          headers: myHeaders,
          body: urlencoded,
          redirect: 'follow'
        };

        return await fetch(baseUrl, requestOptions).then(response => response.text())

      }
    );
  }
}

function createExtraReducers() {
  return {
    ...login(),
  };

  function login() {
    var { pending, fulfilled, rejected } = extraActions.login;
    return {
      [pending]: state => {
        state.error = null;
      },
      [fulfilled]: (state, action) => {
        const user = JSON.parse(action.payload).access_token;
        localStorage.setItem('user', JSON.stringify(user));
        state.user = user;
        const { from } = history.location.state || { from: { pathname: '/' } };
        history.navigate(from);
      },
      [rejected]: (state, action) => {
        state.error = action.error;
      },
    };
  }
}
