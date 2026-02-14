import React, { useEffect } from "react";
import { useStore } from "../Store";
import { Grid, Image } from "semantic-ui-react";
import { observer } from "mobx-react-lite";

export default observer(function Footer() {
  const { categoryStore, newsStore } = useStore();
  const { categories } = categoryStore;

  useEffect(() => {
    categoryStore.loadCategories();
  }, []);

  return (
    <div
      style={{ backgroundColor: "#30486c", marginTop: "100px", color: "#fff" }}
    >
      <Grid>
        <Grid.Row Row={2}></Grid.Row>
        <Grid.Row>
          <Grid.Column centered width={4}>
            {<Image centered src={newsStore?.footerImg} size="small" />}
          </Grid.Column>
          <Grid.Column width={4}>
            <p>
              Ky portal mirëmbahet nga kompania "Telegrafi". Materialet dhe
              informacionet në këtë portal nuk mund të kopjohen, të shtypen, ose
              të përdoren në çfarëdo forme tjetër për qëllime përfitimi, pa
              miratimin e drejtuesve të "Telegrafit". Për ta shfrytëzuar
              materialin e këtij portali obligoheni t'i pranoni Kushtet e
              përdorimit
            </p>
          </Grid.Column>
          <Grid.Column>
            {categories.map((item) => (
              <h4 as="a">{item.name}</h4>
            ))}
          </Grid.Column>
        </Grid.Row>
        <Grid.Row Row={2}></Grid.Row>
        <Grid.Row centered>
          <h4>
            Të gjitha të drejtat janë të rezervuara © 2006-2022 Portali Telegraf
          </h4>
        </Grid.Row>
      </Grid>
    </div>
  );
});
