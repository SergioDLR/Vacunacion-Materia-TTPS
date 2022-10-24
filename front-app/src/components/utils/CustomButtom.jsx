import { Button } from "@mui/material";
const CustomButton = ({
  children,
  variant = "outlined",
  color = "primary",
  textColor = "white",
  fontWeight = 800,
  sx = {},
  type = "button",
  onClick = () => {},
}) => {
  return (
    <Button
      style={{
        borderRadius: 9999,
        color: textColor,
        fontWeight: fontWeight,
        "&:hover": {
          background: "white",
          color: "#111016",
          boxShadow: "none",
        },
      }}
      sx={sx}
      onClick={onClick}
      variant={variant}
      color={color}
      type={type}
    >
      {children}
    </Button>
  );
};

export default CustomButton;
