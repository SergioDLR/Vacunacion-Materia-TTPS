import { Box, Skeleton, Grid } from "@mui/material";

const CustomSkeleton = () => {
  return (
    <Box sx={{ marginTop: 5 }}>
      <Grid container spacing={6}>
        <Grid xs={2}>
          <Skeleton animation="wave" height={100} variant={"text"} />
        </Grid>
        <Grid xs={2}>
          <Skeleton animation="wave" height={100} variant={"text"} />
        </Grid>
      </Grid>
    </Box>
  );
};

export default CustomSkeleton;
