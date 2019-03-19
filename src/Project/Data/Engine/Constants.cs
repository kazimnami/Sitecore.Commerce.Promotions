// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2019
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Project.SamplePromotions.Engine
{
    /// <summary>
    /// The Constants1Constants.
    /// </summary>
    public class Constants
    {
        public static class Operators
        {
            public const string DecimalGreaterThanEqualToOperator = "Sitecore.Framework.Rules.DecimalGreaterThanEqualToOperator";

        }

        public static class DisplayType
        {
            public const string BinaryOperatorDec = "Sitecore.Framework.Rules.IBinaryOperator`2[[System.Decimal],[System.Decimal]], Sitecore.Framework.Rules.Abstractions";
            public const string Decimal = "System.Decimal";
            public const string Integer = "System.Int32";
            public const string String = "System.String";
        }
        /// <summary>
        /// The names of the pipelines.
        /// </summary>
        public static class Pipelines
        {
            // /// <summary>
            // /// The [sample] pipeline name.
            // /// </summary>
            // public const string [sample] = "[project friendly name].Pipeline.[sample]";

            /// <summary>
            /// The names of the pipeline blocks.
            /// </summary>
            public static class Blocks
            {
                // /// <summary>
                // /// The [sample] block name.
                // /// </summary>
                // public const string [sample] = "[project friendly name].Block.[sample]";
            }
        }

        /// <summary>
        /// The names of the routes.
        /// </summary>
        public static class Routes
        {
            ///// <summary>
            ///// The [sample] route.
            ///// </summary>
            //public const string [sample] = "[sample]()";

            /// <summary>
            /// The names of the parameters.
            /// </summary>
            public static class Parameters
            {
                ///// <summary>
                ///// The [sample].
                ///// </summary>
                //public const string [sample] = "[sample]";
            }
        }
    }
}