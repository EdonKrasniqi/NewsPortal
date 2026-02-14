import React from "react";
import { Dimmer, Loader } from "semantic-ui-react";
import LabelNames from "../../Constants/label-names";

export const Loading = () => {
  return (
    <Dimmer active>
      <Loader size="big">{LabelNames.Loading}</Loader>
    </Dimmer>
  );
};
