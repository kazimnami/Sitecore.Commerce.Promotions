DECLARE @CustomPromotions TABLE (Id VARCHAR(MAX), Coupon VARCHAR(MAX))
INSERT INTO @CustomPromotions VALUES ('Entity-Promotion-Habitat_PromotionBook-_Cart5OffDeliveryShippingOptionPromotion', 'Entity-Coupon-SHIPPINGDISCOUNT')
INSERT INTO @CustomPromotions VALUES ('Entity-Promotion-Habitat_PromotionBook-_LineBrandAmountDiscountPromotion', 'Entity-Coupon-10OFFELECTRONICS')
INSERT INTO @CustomPromotions VALUES ('Entity-Promotion-Habitat_PromotionBook-_LineCategoryAmountDiscountPromotion', 'Entity-Coupon-10OFFAUDIO')
INSERT INTO @CustomPromotions VALUES ('Entity-Promotion-Habitat_PromotionBook-_LineCategoryPercentDiscountPromotion', 'Entity-Coupon-10PCTOFFAUDIO')
INSERT INTO @CustomPromotions VALUES ('Entity-Promotion-Habitat_PromotionBook-_LineQuantityPctOffPromotion', 'Entity-Coupon-RANGEPCTOFF')
INSERT INTO @CustomPromotions VALUES ('Entity-Promotion-Habitat_PromotionBook-_LineTagAmountDiscountPromotion', 'Entity-Coupon-10OFFEARBUDS')
INSERT INTO @CustomPromotions VALUES ('Entity-Promotion-Habitat_PromotionBook-_LineTagPercentDiscountPromotion', 'Entity-Coupon-10PCTOFFEARDBUDS')
INSERT INTO @CustomPromotions VALUES ('Entity-Promotion-Habitat_PromotionBook-_LineXQuantityForPricePromotion', 'Entity-Coupon-3FOR50')
INSERT INTO @CustomPromotions VALUES ('Entity-Promotion-Habitat_PromotionBook-_LineXQuantityForYQuantityPromotion', 'Entity-Coupon-4FOR3')

DELETE
FROM [SitecoreCommerce9_SharedEnvironments].[dbo].[PromotionsEntities]
WHERE Id IN (SELECT Id FROM @CustomPromotions)

DELETE
FROM [SitecoreCommerce9_SharedEnvironments].[dbo].[PromotionsLists]
WHERE CommerceEntityId IN (SELECT Id FROM @CustomPromotions)

DELETE
FROM [SitecoreCommerce9_SharedEnvironments].[dbo].[CommerceEntities]
WHERE Id IN (SELECT Coupon FROM @CustomPromotions)

DELETE
FROM [SitecoreCommerce9_SharedEnvironments].[dbo].[CommerceLists]
WHERE CommerceEntityId IN (SELECT Coupon FROM @CustomPromotions)
