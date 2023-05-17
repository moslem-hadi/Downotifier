import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';

import { fetchWrapper } from '../_helpers';

// create slice

const name = 'singleJobApiCall';
const initialState = createInitialState();
const extraActions = createExtraActions();
const extraReducers = createExtraReducers();
const slice = createSlice({ name, initialState, extraReducers });

// exports

export const singleJobApiCallActions = { ...slice.actions, ...extraActions };
export const singleJobApiCallReducer = slice.reducer;

// implementation

function createInitialState() {
  return {
    jobApiCall: {
      loading: true,
    },
  };
}

function createExtraActions() {
  const baseUrl = `${process.env.REACT_APP_API_URL}jobApiCall`;

  return {
    GetById: GetById(), 
  };

  function GetById() {
    return createAsyncThunk(
      `${name}/GetById`,
      async ({ id }, thunkApi) =>
        await fetchWrapper.get(`${baseUrl}/${id}`)
    );
  }
}

function createExtraReducers() {
  return {
    ...GetById(),
  };

  function GetById() {
    var { pending, fulfilled, rejected } = extraActions.GetById;
    return {
      [pending]: state => {
        state.jobApiCall = { loading: true };
      },
      [fulfilled]: (state, action) => {
        state.jobApiCall = action.payload;
      },
      [rejected]: (state, action) => {
        state.jobApiCall = { error: action.error };
      },
    };
  }
}
