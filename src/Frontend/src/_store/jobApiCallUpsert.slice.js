import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';

import { fetchWrapper } from '../_helpers';

// implementation
function createInitialState() {
  return {
    loading: true,
    done: false,
  };
}

function createExtraActions() {
  const baseUrl = `${process.env.REACT_APP_API_URL}jobApiCall`;

  return {
    createJobApiCall: createJobApiCall(),
  };

  function createJobApiCall() {
    return createAsyncThunk(
      `${name}/post`,
      async (model, thunkApi) => {
        if (model.headers?.length > 0)
          model.headers = getParameters(model.headers);
        else
          model.headers = null;
        model.method = parseInt(model.method)
        if (model.id) return await fetchWrapper.put(`${baseUrl}/${model.id}`, model);
        else return await fetchWrapper.post(baseUrl, model);
      }
    );
  }
}
function getParameters(paramStr) {
  var paramStr = paramStr.split("&");
  var parameters = {}
  paramStr.map(item => {
    let keyValue = item.split(':')
    return parameters[keyValue[0]] = keyValue[1];
  })
  return parameters;
}

function createExtraReducers() {
  return {
    ...createJobApiCall(),
  };

  function createJobApiCall() {
    var { pending, fulfilled, rejected } = extraActions.createJobApiCall;
    return {
      [pending]: state => {
        state = { loading: true };
      },
      [fulfilled]: (state, action) => {
        state = { done: true };
      },
      [rejected]: (state, action) => {
        state = { error: 'error happened' };
      },
    };
  }
}

// create slice

const name = 'jobApiCallUpsert';
const initialState = createInitialState();
const extraActions = createExtraActions();
const extraReducers = createExtraReducers();
const slice = createSlice({ name, initialState, extraReducers });

// exports

export const jobApiCallUpsertActions = { ...slice.actions, ...extraActions };
export const jobApiCallUpsertReducer = slice.reducer;
